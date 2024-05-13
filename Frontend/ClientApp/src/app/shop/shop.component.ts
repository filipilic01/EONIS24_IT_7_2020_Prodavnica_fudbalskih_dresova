import { Component, OnInit, ViewChild,ElementRef } from '@angular/core';
import { Dres } from '../shared/models/dres';
import { ShopService } from './shop.service';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit{
  @ViewChild('search') searchTerm?: ElementRef
dresovi:Dres[] = []
seasonSelected = ''
typeSelected = ''
sortSelected = ''
totalCount = 0
pageSize = 6
pageIndex = 1
search = ''


constructor(private shopService: ShopService){

}

  ngOnInit(): void {
 this.getDresovi();
  }

  getDresovi() {
    this.shopService.getDresovi(this.seasonSelected, this.typeSelected, this.sortSelected,this.pageSize, this.pageIndex,this.search).subscribe({
      next: response => {
        this.dresovi = response.data
        this.pageSize = response.pageSize
        this.pageIndex = response.pageIndex
        this.totalCount = response.count
      },
      error: error => console.error()
      
    })  
  }

  onSeasonSelected(event: any) {
    const value = event.target.value;
    this.seasonSelected =value;
    this.pageIndex =1
   this.getDresovi();
  }

  onTypeSelected(event: any){
    const value = event.target.value;
    this.typeSelected = value;
    this.pageIndex =1
    this.getDresovi();
  }

  onSortSelected(event: any){
    const value = event.target.value;
    this.sortSelected = value;
    this.pageIndex =1
    this.getDresovi();
  }

  onPageChanged(event: any){
    if (this.pageIndex !== event.page){
      this.pageIndex = event.page
      this.getDresovi();
    }
  }

  onSearch(){
    this.search = this.searchTerm?.nativeElement.value;
    this.pageIndex =1
    this.getDresovi();
  }

  onReset(){
    if(this.searchTerm){
      this.searchTerm.nativeElement.value=''
    }
    this.seasonSelected=''
    this.sortSelected=''
    this.typeSelected=''
    this.pageSize=6
    this.pageIndex=1
    this.search=''
    
    const sortSelect = document.getElementById('sortSelect') as HTMLSelectElement;
    sortSelect.selectedIndex = 0;

    const seasonSelect = document.getElementById('seasonSelect') as HTMLSelectElement;
    seasonSelect.selectedIndex = 0;

    const typeSelect = document.getElementById('typeSelect') as HTMLSelectElement;
    typeSelect.selectedIndex = 0;
    this.getDresovi()
  }
}
