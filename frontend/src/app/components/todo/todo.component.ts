import { Component, OnInit } from '@angular/core';
import { TodoService } from '../../services/todo.service';
import { Todo } from '../../models/todo';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-todo',
  templateUrl: './todo.component.html',
  styleUrls: ['./todo.component.css']
})
export class TodoComponent implements OnInit {
  todos: Todo[] = [];
  todoForm: FormGroup;
  loading = false;
  error: string | null = null;
  editingTodo: Todo | null = null;

  constructor(
    private todoService: TodoService,
    private fb: FormBuilder
  ) {
    this.todoForm = this.fb.group({
      title: ['', [Validators.required, Validators.minLength(3)]],
      description: ['', Validators.required],
      completed: [false]
    });
  }

  ngOnInit(): void {
    this.loadTodos();
  }

  loadTodos(): void {
    this.loading = true;
    this.error = null;
    this.todoService.getTodos().subscribe({
      next: (todos) => {
        this.todos = todos;
        this.loading = false;
      },
      error: (error) => {
        this.error = 'Failed to load todos. Please try again later.';
        this.loading = false;
        console.error('Error loading todos:', error);
      }
    });
  }

  onSubmit(): void {
    if (this.todoForm.valid) {
      this.loading = true;
      this.error = null;
      
      if (this.editingTodo) {
        this.todoService.updateTodo(this.editingTodo.id, this.todoForm.value).subscribe({
          next: (updatedTodo) => {
            const index = this.todos.findIndex(t => t.id === updatedTodo.id);
            if (index !== -1) {
              this.todos[index] = updatedTodo;
            }
            this.resetForm();
            this.loading = false;
          },
          error: (error) => {
            this.error = 'Failed to update todo. Please try again.';
            this.loading = false;
            console.error('Error updating todo:', error);
          }
        });
      } else {
        this.todoService.createTodo(this.todoForm.value).subscribe({
          next: (newTodo) => {
            this.todos.unshift(newTodo);
            this.resetForm();
            this.loading = false;
          },
          error: (error) => {
            this.error = 'Failed to create todo. Please try again.';
            this.loading = false;
            console.error('Error creating todo:', error);
          }
        });
      }
    }
  }

  toggleTodo(todo: Todo): void {
    this.loading = true;
    this.error = null;
    this.todoService.toggleTodoStatus(todo.id, !todo.completed).subscribe({
      next: (updatedTodo) => {
        const index = this.todos.findIndex(t => t.id === updatedTodo.id);
        if (index !== -1) {
          this.todos[index] = updatedTodo;
        }
        this.loading = false;
      },
      error: (error) => {
        this.error = 'Failed to update todo status. Please try again.';
        this.loading = false;
        console.error('Error toggling todo:', error);
      }
    });
  }

  deleteTodo(id: number): void {
    if (confirm('Are you sure you want to delete this todo?')) {
      this.loading = true;
      this.error = null;
      this.todoService.deleteTodo(id).subscribe({
        next: () => {
          this.todos = this.todos.filter(todo => todo.id !== id);
          this.loading = false;
        },
        error: (error) => {
          this.error = 'Failed to delete todo. Please try again.';
          this.loading = false;
          console.error('Error deleting todo:', error);
        }
      });
    }
  }

  editTodo(todo: Todo): void {
    this.editingTodo = todo;
    this.todoForm.patchValue({
      title: todo.title,
      description: todo.description,
      completed: todo.completed
    });
  }

  resetForm(): void {
    this.todoForm.reset({ completed: false });
    this.editingTodo = null;
  }
} 