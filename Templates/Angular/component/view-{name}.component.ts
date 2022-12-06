import { Component, OnInit } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { Users } from 'src/app/model';
import { [TABLE_NAME] } from 'src/app/model/[TABLE_NAME]';
import { ContentBlobObject } from 'src/app/model/ContentBlobObject';
import { Content, ContentTemplateModel } from 'src/app/model/ContentTemplateModel';
import { AppdataService } from 'src/app/service/appdata.service';
import { AuthenticationService } from 'src/app/service/authentication.service';
import { ContentService } from 'src/app/service/content.service';
import { MyworldService } from 'src/app/service/myworld.service';
import { constants } from 'src/app/utility/constants';
import { utility } from 'src/app/utility/utility';

@Component({
  selector: 'app-view-[TABLE_NAME_SMALL]',
  templateUrl: './view-[TABLE_NAME_SMALL].component.html',
  styleUrls: ['./view-[TABLE_NAME_SMALL].component.scss']
})
export class View[TABLE_NAME]Component implements OnInit {
  id: any = "";
  contentAvailable: boolean = false;
  [TABLE_NAME_S_SMALL]Dic: { [key: string]: string } = {};
  headerBackgroundColor: any;
  ContentTemplate: Content = new Content();
  ContentObjectList: ContentBlobObject[] = [];
  Constants = constants;

  constructor(private activatedRoute: ActivatedRoute,
    private appdataService: AppdataService,
    private authService: AuthenticationService,
    private myworldService: MyworldService,
    private contentService: ContentService,
    private sanitized: DomSanitizer,
    private router: Router) {
    this.id = this.activatedRoute.snapshot.paramMap.get('id');
  }

  ngOnInit(): void {
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
          this.contentAvailable = true;
        }
        else {
          this.router.navigate(["404"]);
        }
      }
    });

    this.myworldService.getContentTypes('[TABLE_NAME]').subscribe(res => {
      this.headerBackgroundColor = res.sec_color;
    });

    this.myworldService.getContentBlobObject(this.id, '[TABLE_NAME_SMALL]').subscribe(res => {
      if(res != null){
        res.map(c => {
          const blob = utility.b64toBlob(c.object_blob, c.content_type);
          let url = window.URL.createObjectURL(blob);              
          c.image_url = url;
          this.ContentObjectList.push(c);
        });
      }
    });

    this.myworldService.getUsersContentTemplate(accountId).subscribe(res => {
      let contentTemplateModel = JSON.parse(res.template) as ContentTemplateModel;
      this.ContentTemplate = contentTemplateModel.contents.find(c => c.content_type == "[TABLE_NAME_SMALL]")!;
      console.log(this.ContentTemplate);
      this.ContentTemplate.categories = this.ContentTemplate.categories.sort((a, b) => a.order - b.order);
      this.ContentTemplate.references = this.ContentTemplate.references.sort((a, b) => a.order - b.order);
    });
  }

  senitizeHtmlContent(value: any) {
    const html = this.sanitized.bypassSecurityTrustHtml(value);
    return html;
  }
}
