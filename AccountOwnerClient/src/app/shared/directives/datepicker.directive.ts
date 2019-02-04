import { Directive, Output, OnInit, ElementRef, EventEmitter } from "@angular/core";
//import { EventEmitter } from 'events';

@Directive({
    selector: '[appDatepicker]'
})

export class DatepickerDirective implements OnInit {
    @Output() public change = new EventEmitter();

    constructor(private elementRef: ElementRef) { }

    public ngOnInit() {
        $(this.elementRef.nativeElement).datepicker({
            dateFormat: 'mm/dd/yy',
            changeYear: true,
            yearRange: "-100:+0",
            onSelect: (dateText) => {
                this.change.emit(dateText);
            }
        });
    }
}