import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { NgIf } from '@angular/common';
import { TodoService } from '../../services/todo.service';

@Component({
  selector: 'app-todo-form',
  templateUrl: './todo-form.component.html',
  styleUrls: ['./todo-form.component.scss'],
  imports: [ReactiveFormsModule, NgIf],
  standalone: true
})
export class TodoFormComponent implements OnInit {
  todoForm: FormGroup;
  isEditMode = false;
  todoId?: number;

  constructor(
    private fb: FormBuilder,
    private todoService: TodoService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.todoForm = this.fb.group({
      title: ['', [Validators.required, Validators.minLength(3)]],
      description: [''],
      completed: [false]
    });
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.isEditMode = true;
        this.todoId = +params['id'];
        this.loadTodo();
      }
    });
  }

  get title() {
    return this.todoForm.get('title')!;
  }

  loadTodo(): void {
    if (this.todoId) {
      this.todoService.getTodoById(this.todoId).subscribe({
        next: (todo) => {
          this.todoForm.patchValue({
            title: todo.title,
            description: todo.description,
            completed: todo.completed
          });
        },
        error: (error) => {
          console.error('Error loading todo:', error);
          this.router.navigate(['/todos']);
        }
      });
    }
  }

  onSubmit(): void {
    if (this.todoForm.valid) {
      const todoData = this.todoForm.value;
      
      if (this.isEditMode && this.todoId) {
        this.todoService.updateTodo(this.todoId, todoData).subscribe({
          next: () => this.router.navigate(['/todos']),
          error: (error) => console.error('Error updating todo:', error)
        });
      } else {
        this.todoService.createTodo(todoData).subscribe({
          next: () => this.router.navigate(['/todos']),
          error: (error) => console.error('Error creating todo:', error)
        });
      }
    }
  }

  onCancel(): void {
    this.router.navigate(['/todos']);
  }
}
