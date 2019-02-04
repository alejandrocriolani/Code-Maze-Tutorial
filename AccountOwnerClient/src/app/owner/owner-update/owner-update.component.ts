import { Component, OnInit } from '@angular/core';
import { Owner } from 'src/app/_interfaces/owner.model';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { RepositoryService } from 'src/app/shared/services/repository.service';
import { ErrorHandleService } from 'src/app/shared/services/error-handle.service';
import { Router, ActivatedRoute } from '@angular/router';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-owner-update',
  templateUrl: './owner-update.component.html',
  styleUrls: ['./owner-update.component.css']
})
export class OwnerUpdateComponent implements OnInit {
  public errorMessage: string = '';
  public owner: Owner;
  public ownerForm: FormGroup;

  constructor(private repository: RepositoryService,
              private errorHandler: ErrorHandleService,
              private router: Router,
              private activeRoute: ActivatedRoute,
              private datePipe: DatePipe) { }

  ngOnInit() {
    this.ownerForm = new FormGroup({
      name: new FormControl('', [Validators.required, Validators.maxLength(60)]),
      dateOfBirth: new FormControl('', [Validators.required]),
      address: new FormControl('', [Validators.required, Validators.maxLength(100)])
    });

    this.getOwnerById();
  }

  private getOwnerById() {
    let ownerId: string = this.activeRoute.snapshot.params['id'];
    let ownerByIdUrl: string = `api/owner/${ownerId}`;

    this.repository.getData(ownerByIdUrl)
      .subscribe(res=> {
        this.owner = res as Owner;
        this.ownerForm.patchValue(this.owner);
        $('#dateOfBirth').val(this.datePipe.transform(this.owner.dateOfBirth, 'MM/dd/yyyy'));
      },
      (error) => {
        this.errorHandler.handleError(error);
        this.errorMessage = this.errorHandler.errorMessage;
      })
  }

  public validateControl(controlName: string){
    if(this.ownerForm.controls[controlName].invalid && this.ownerForm.controls[controlName].touched)
      return true;
    return false;
  }

  public hasError(controlName: string, errorName: string){
    if(this.ownerForm.controls[controlName].hasError(errorName))
      return true;
    return false;
  }

  public executeDatePicker(event){
    this.ownerForm.patchValue({'dateOfBirth':event});
  }

  public redirectToOwnerList(){
    this.router.navigate(['/owner/list']);
  }

  public updateOwner(ownerFormValue) {
    if (this.ownerForm.valid){
      this.executeOwnerUpdate(ownerFormValue);
    }
  }

  public executeOwnerUpdate(ownerFormValue){
    this.owner.name = ownerFormValue.name,
      this.owner.dateOfBirth = ownerFormValue.dateOfBirth,
      this.owner.address = ownerFormValue.address

    console.log("ExecuteOwnerUpdate()");
    let apiUrl = `api/owner/${this.owner.id}`;
    console.log(apiUrl);
    console.log(this.owner);
    this.repository.update(apiUrl, this.owner)
      .subscribe(res => {
        $('#successModal').modal();
      },
      (error=> {
        this.errorHandler.handleError(error);
        this.errorMessage = this.errorHandler.errorMessage;
        console.log(this.errorMessage);
      })
    )
  }
}
