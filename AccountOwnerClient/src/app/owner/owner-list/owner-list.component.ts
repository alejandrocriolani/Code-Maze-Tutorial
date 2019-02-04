import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { RepositoryService } from './../../shared/services/repository.service';
import { Owner } from './../../_interfaces/owner.model';
import { ErrorHandleService } from 'src/app/shared/services/error-handle.service';
import { NotFoundComponent } from 'src/app/error-pages/not-found/not-found.component';
import { routerNgProbeToken } from '@angular/router/src/router_module';

@Component({
  selector: 'app-owner-list',
  templateUrl: './owner-list.component.html',
  styleUrls: ['./owner-list.component.css']
})
export class OwnerListComponent implements OnInit {

  public owners: Owner[];
  public errorMessage: string = '';

  constructor(private repository: RepositoryService,
              private errorHandler: ErrorHandleService,
              private router: Router) { }

  ngOnInit() {
    this.getAllOwners();
  }

  public getAllOwners(){
    let apiAddress : string = "api/owner";
    this.repository.getData(apiAddress)
      .subscribe(res => {
        this.owners = res as Owner[]
      },
      (error) => {
        this.errorHandler.handleError(error);
        this.errorMessage = this.errorHandler.errorMessage;
      })
  }

  public getOwnerDetails(id){
    let detailsUrl: string = `/owner/details/${id}`;
    this.router.navigate([detailsUrl]);
  }

  public redirectToUpdatePage(id){
    let updateUrl: string = `/owner/update/${id}`;
    this.router.navigate([updateUrl]);
  }

  public redirectToDeletePage(id){
    let updateUrl: string = `/owner/delete/${id}`;
    this.router.navigate([updateUrl]);
  }

}
