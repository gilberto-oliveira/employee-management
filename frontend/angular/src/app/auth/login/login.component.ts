import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { FloatLabel } from 'primeng/floatlabel';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, ButtonModule, InputTextModule, CommonModule, FloatLabel, ProgressSpinnerModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {

  loginForm: FormGroup;

  loading: boolean = false;

  errorMessage: string | null = null;

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  async onSubmit() {
    if (this.loginForm.valid) {
      const { username, password } = this.loginForm.value;

      try {
        this.loading = true;
        const response = await this.authService.authenticate(username, password);
        console.log('Login successful:', response);
        this.router.navigate(['/modules/employees']);
        this.errorMessage = null;
      } catch (error) {
        console.error('Login failed:', error);
        this.errorMessage = 'Invalid credentials. Please try again.';  // Show error message
      } finally {
        this.loading = false;  // Hide the loading spinner after the request is done
      }

    }
  }
}
