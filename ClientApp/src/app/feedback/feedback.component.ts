import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Component({
  selector: 'app-feedback',
  templateUrl: './feedback.component.html',
  styleUrls: ['./feedback.component.scss']
})
export class FeedbackComponent implements OnInit {

  loading = false;

  form = new FormGroup({
    name: new FormControl('', [
      Validators.required,
    ]),
    email: new FormControl('', [Validators.required, Validators.email]),
    text: new FormControl('', [Validators.required])
  });

  constructor(private http: HttpClient,
    @Inject('BASE_URL') public baseUrl: string,
    private snackbar: MatSnackBar,
  ) { }

  ngOnInit(): void {
  }

  submit(): void {
    this.loading = true;

    this.http.post(this.baseUrl + 'mail', this.form.value).pipe(
      catchError((err) => {
        this.snackbar.open('Произошла ошибка: ' + err?.message, 'Закрыть');
        this.loading = false;
        return throwError(new Error(err?.name));
      }),
    ).subscribe({
      next: () => {
        this.snackbar.open('Сообщение отправлено', 'Закрыть', {});
        this.form.reset();
        this.loading = false;
      }
    })
  };
}

