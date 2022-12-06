import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Users } from 'src/app/model';
import { [TABLE_NAME] } from 'src/app/model/[TABLE_NAME]';
import { AuthenticationService } from 'src/app/service/authentication.service';
import { ContentService } from 'src/app/service/content.service';
import { MyworldService } from 'src/app/service/myworld.service';
import { constants } from 'src/app/utility/constants';
import { utility } from 'src/app/utility/utility';

@Component({
  selector: 'app-[TABLE_NAME_SMALL]',
  templateUrl: './[TABLE_NAME_SMALL].component.html',
  styleUrls: ['./[TABLE_NAME_SMALL].component.css']
})
export class [TABLE_NAME]Component implements OnInit {

  public [TABLE_NAME_SMALL]: [TABLE_NAME][] = [];
  public currentDeleteId: any;
    constructor(private authService: AuthenticationService, private router: Router, private myworldService: MyworldService,
    private contentService: ContentService) {

  }
  ngOnInit(): void {
    this.index();
  }

  index(): void {
    this.[TABLE_NAME_SMALL] = [];
    let accountId = (this.authService.getUser() as (Users)).id;
    console.log(accountId);
    let imageFormat = this.authService.getValue(constants.ContentImageUrlFormat);
    console.log(imageFormat);
    this.contentService.getAll[TABLE_NAME](accountId).subscribe({
      next: (res) => {
        res.map(b => {
          this.myworldService.getContentBlobObject(b.id, '[TABLE_NAME_SMALL]').subscribe(conObjectResponse => {
            if (conObjectResponse) {
              let firstObject = conObjectResponse[0];
              const blob = utility.b64toBlob(firstObject.object_blob, firstObject.content_type);
              let url = window.URL.createObjectURL(blob);              
              b.image_url = url;
            }
            else {
               b.image_url = "assets/img/cards/[TABLE_NAME].png";
              console.log(b);
            }
            this.[TABLE_NAME_SMALL].push(b);
            this.[TABLE_NAME_SMALL] = this.[TABLE_NAME_SMALL].sort((a, b) => a.id! - b.id!);
          });
        });
      }
    });
  }

    onDelete(id: any): void {
        console.log("on delete for id " + id);
        this.currentDeleteId = id;
    }

    delete[TABLE_NAME_S](option : string){
        if (option == "YES") {
            let [TABLE_NAME_S_SMALL] = new [TABLE_NAME]();
            [TABLE_NAME_S_SMALL].id = this.currentDeleteId;
            this.contentService.delete[TABLE_NAME_S]([TABLE_NAME_S_SMALL]).subscribe({
                next: response => {
                    console.log(response);
                    this.index();
                }
            });
        }
        this.currentDeleteId = 0;
    }
}
