import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LinkedUserComponent } from './linked-user.component';

describe('LinkedUserComponent', () => {
  let component: LinkedUserComponent;
  let fixture: ComponentFixture<LinkedUserComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LinkedUserComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LinkedUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
