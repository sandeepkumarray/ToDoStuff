import { Component, OnInit} from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpEventType, HttpResponse } from '@angular/common/http';
import { TabContentComponent } from '@coreui/angular';
import { Users } from 'src/app/model';
import { Content, ContentTemplateModel } from 'src/app/model/ContentTemplateModel';
import { BaseModel } from "src/app/model/BaseModel";
import { ContentBlobObject } from 'src/app/model/ContentBlobObject';
import { AppdataService } from 'src/app/service/appdata.service';
import { AuthenticationService } from 'src/app/service/authentication.service';
import { ContentService } from 'src/app/service/content.service';
import { MyworldService } from 'src/app/service/myworld.service';
import { DocumentService } from 'src/app/service/document.service';
import { constants } from 'src/app/utility/constants';
import { utility } from 'src/app/utility/utility';
import 'quill-mention'

@Component({
  selector: 'app-edit-[TABLE_NAME_SMALL]',
  templateUrl: './edit-[TABLE_NAME_SMALL].component.html',
  styleUrls: ['./edit-[TABLE_NAME_SMALL].component.css']
})
export class Edit[TABLE_NAME]Component implements OnInit {
  id: any = "";
  [TABLE_NAME_S_SMALL]Dic: { [key: string]: string } = {};
  headerBackgroundColor: any;
  ContentTemplate: Content = new Content();
  ContentObjectList: ContentBlobObject[] = [];
  Constants = constants;
  remainingSize?: string;
  expandAllToggle: boolean = false;
  allAttachments: ContentBlobObject[] = [];

  slides: any[] =[];

  modules = {
    toolbar: false,
    mention: {
      allowedChars: /^[A-Za-z\sÅÄÖåäö]*$/,
      mentionDenotationChars: ["@"],
      showDenotationChar: false,
      onSelect: (item: any, insertItem: (arg0: any) => void) => {
        // const editor = this.editor.quillEditor;
        insertItem(item);
        // // necessary because quill-mention triggers changes as 'api' instead of 'user'
        // editor.insertText(editor.getLength() - 1, "", "user");
      },
      source: (searchTerm: string, renderList: (arg0: any[], arg1: any) => void) => {
        let accountId = (this.authService.getUser() as (Users)).id;
        this.documentService.getAllMentions(accountId).subscribe({
          next: response => {
            console.log(response);
            let matches: any[] = [];
            response.map(m => {
              let content: any = {};
              content.id = m.id;
              content.icon = m.icon!.replace("fa-3x", "fa-2x");
              content.label = m.name;
              content.link = "#/" + m.content_type + "/" + m.id;
              content.value = "[" + m.content_type + "-" + m.id + ":" + m.name + "]";
              matches.push(content);
            });
            //renderList(response.filter(content => content.name.includes(searchTerm)));
            renderList(matches.filter(content => content.value.includes(searchTerm)), searchTerm);
          }
        });
      },
      renderItem: (item: any, searchTerm: any) => {
        console.log(item);
        return '<div class="d-flex justify-content-between align-items-center"><span style="font-size: 14px;">' + item.label + '</span><span style="margin-left:20px">' + item.icon + '</span></div>';
      }
    }
  };

  constructor(private activatedRoute: ActivatedRoute,
    private appdataService: AppdataService,
    private authService: AuthenticationService,
    private myworldService: MyworldService,
    private contentService: ContentService,
    private documentService: DocumentService,
    private sanitized: DomSanitizer,
    private router: Router) {
    this.id = this.activatedRoute.snapshot.paramMap.get('id');
  }

  ngOnInit(): void {
    this.slides[0] = {
      src: './assets/img/cards/[TABLE_NAME].png',
    };
    this.slides[1] = {
      src: './assets/img/cards/Characters.png',
    }
    this.slides[2] = {
      src: './assets/img/cards/Conditions.png',
    }
    
    let allowedTotalContentSize = this.authService.getValue(constants.AllowedTotalContentSize);
    this.remainingSize = utility.SizeSuffix(allowedTotalContentSize);
    let accountId = (this.authService.getUser() as (Users)).id;
    this.contentService.get[TABLE_NAME](accountId, this.id).subscribe({
      next: response => {
        if (response != null) {
          var jsonString = JSON.stringify(response);
          var jsonObject = JSON.parse(jsonString);
          var dictionary: { [key: string]: string } = {};
          Object.keys(jsonObject).map(function (key) {
            dictionary[(key)] = jsonObject[key];
          });
          this.[TABLE_NAME_S_SMALL]Dic = dictionary;
          console.log(dictionary);
        }
        else {
          this.router.navigate(["404"]);
        }
      }
    });

    this.myworldService.getContentTypes('[TABLE_NAME]').subscribe(res => {
      this.headerBackgroundColor = res.sec_color;
    });

    this.loadContentBlobObjects();

    this.myworldService.getUsersContentTemplate(accountId).subscribe(res => {
      let contentTemplateModel = JSON.parse(res.template) as ContentTemplateModel;
      this.ContentTemplate = contentTemplateModel.contents.find(c => c.content_type == "[TABLE_NAME_SMALL]")!;
      console.log(this.ContentTemplate);

      this.ContentTemplate.categories = this.ContentTemplate.categories.sort((a, b) => a.order - b.order);
      this.ContentTemplate.references = this.ContentTemplate.references.sort((a, b) => a.order - b.order);

      var catIndex = 0;
      this.ContentTemplate.categories.map(c => {
        c.index = catIndex++;
        c.is_active = this.setActive(c.order);
      });
      catIndex = 0;
      this.ContentTemplate.references.map(c => {
        c.index = catIndex++;
        c.is_active = this.setActive(c.order);
      });
    });
  }

  loadContentBlobObjects() {
    this.ContentObjectList = [];
    this.myworldService.getContentBlobObject(this.id, '[TABLE_NAME_SMALL]').subscribe(res => {
      let allowedTotalContentSize = this.authService.getValue(constants.AllowedTotalContentSize);
      let existing_total_size: number = 0;
      if (res != null) {
        res.map(c => {
          const blob = utility.b64toBlob(c.object_blob, c.content_type);
          let url = URL.createObjectURL(blob);
          c.image_url = url;
          c.safeurl = this.sanitized.bypassSecurityTrustResourceUrl('data:image/png;base64,' + c.object_blob);
          c.size = utility.SizeSuffix(c.object_size!);
          existing_total_size = +existing_total_size + +c.object_size!;
          this.ContentObjectList.push(c);
        });

        this.remainingSize = utility.SizeSuffix(allowedTotalContentSize - existing_total_size);
        console.log(this.remainingSize);
      }
    });

  }

  setActive(order: number): boolean {
    if (order == 1) {
      return true;
    }
    else
      return false;
  }

  expandAll(tabContent: TabContentComponent) {
    const count = tabContent.panes.filter((obj) => obj.active === true).length;

    if (this.expandAllToggle == false) {
      this.expandAllToggle = true;
      this.ContentTemplate.categories.forEach(category => {
        var elem = document.getElementById(category.label + "_panel");
        var elemTabPane = document.getElementById(category.label + "_tabpane");
        elem?.classList.add("active");
        elemTabPane?.classList.add("active");
        elemTabPane?.classList.add("show");
      });
      // tabContent.panes.forEach(tabPane => {
      //   tabPane.active = true;
      // });
    }
    else {
      this.expandAllToggle = false;
      this.ContentTemplate.categories.forEach(category => {
        var elem = document.getElementById(category.label + "_panel");
        var elemTabPane = document.getElementById(category.label + "_tabpane");
        elem?.classList.remove("active");
        elemTabPane?.classList.remove("active");
        elemTabPane?.classList.remove("show");
      });

      this.ContentTemplate.references.forEach(reference => {
        var elem = document.getElementById(reference.label + "_panel");
        var elemTabPane = document.getElementById(reference.label + "_tabpane");
        elem?.classList.remove("active");
        elemTabPane?.classList.remove("active");
        elemTabPane?.classList.remove("show");
      });

      this.ContentTemplate.categories.forEach(category => {
        var elem = document.getElementById(category.label + "_panel");
        var elemTabPane = document.getElementById(category.label + "_tabpane");
        console.log(elemTabPane);
        if (category.index == 0) {
          if (!elem?.classList.contains("active")) {
            elem?.classList.add("active");
            elemTabPane?.classList.add("active");
            elemTabPane?.classList.add("show");
          }
        }
      });

    }
  }

    checkValue($event: any, field_name: string){
        console.log('checkValue', $event.target.checked)
        console.log('checkValue', field_name)

        let value = Number($event.target.checked);
        console.log('checkValue value', value)

        let model: BaseModel = new BaseModel();
        model._id = this.id;
        model.column_type = field_name;
        model.column_value = value;
        model.content_type = "[TABLE_NAME_SMALL]";
        this.contentService.saveData(model).subscribe({
            next: response => {

                console.log(response);
            }
        });
    }

  onBlur($event: any, field_name: string) {
    console.log('blur', $event)
    console.log('blur', field_name)

    let model: BaseModel = new BaseModel();
    model._id = this.id;
    model.column_type = field_name;
    model.column_value = $event.target.value;
    model.content_type = "[TABLE_NAME_SMALL]";
    this.contentService.saveData(model).subscribe({
      next: response => {

        console.log(response);
      }
    });
  }

  onQuillBlur($event: any, field_name: string) {
    console.log('blur', $event)
    console.log('blur', field_name)

    let model: BaseModel = new BaseModel();
    model._id = this.id;
    model.column_type = field_name;
    model.column_value = $event.editor.root.innerHTML;
    model.content_type = "[TABLE_NAME_SMALL]";
    this.contentService.saveData(model).subscribe({
      next: response => {

        console.log(response);
      }
    });
  }

  addImage(event: any) {
    console.log(event);
    let accountId = (this.authService.getUser() as (Users)).id;
    this.allAttachments = [];
    let files: FileList = event.target.files;
    for (let i = 0; i < files.length; i++) {
      let aFile: ContentBlobObject = new ContentBlobObject();
      aFile.object_blob = files[i];
      aFile.inProgress = false;
      aFile.progress = 0;
      aFile.object_type = files[i].type;
      aFile.object_name = files[i].name;
      aFile.object_size = files[i].size;
      aFile.content_id = this.id;
      aFile.user_id = Number(accountId!);
      aFile.content_type = "[TABLE_NAME_SMALL]";
      this.allAttachments.push(aFile);
    }
  }

  uploadImage() {
    let allowedTotalContentSize = this.authService.getValue(constants.AllowedTotalContentSize);
    this.remainingSize = utility.SizeSuffix(allowedTotalContentSize);
    let existing_total_size: number = 0;
    this.ContentObjectList.forEach(a => existing_total_size += a.object_size!);
    if (this.allAttachments != null) {

      let upload_file_size: number = 0;
      this.allAttachments.forEach(a => upload_file_size += a.object_size!);
      var total_size = upload_file_size + existing_total_size;
      if (total_size <= allowedTotalContentSize) {

        for (let i = 0; i < this.allAttachments.length; i++) {

          this.myworldService.addImageToContent(this.allAttachments[i])
            .subscribe(
              event => {
                if (event.type == HttpEventType.UploadProgress) {
                  const percentDone = Math.round(100 * event.loaded / event.total!);
                  console.log(`File is ${percentDone}% loaded.`);
                } else if (event instanceof HttpResponse) {
                  console.log('File is completely loaded!');
                  //this.loadAttachments();
                  this.loadContentBlobObjects();
                }
              },
              (err) => {
                console.log("Upload Error:", err);
              }, () => {
                console.log("Upload done");
              }
            )
        }

      }
    }
    else{
      //Messagebox
    }
  }

  deleteContentObject(object_id: any) {
    this.myworldService.deleteContentBlobObject(object_id).subscribe(res => {
      console.log(res);
      window.location.reload();
    });
  }
}
