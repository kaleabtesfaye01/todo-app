import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgFor, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Todo } from '../../models/todo';
import { TodoService } from '../../services/todo.service';
import { TodoItemComponent } from '../todo-item/todo-item.component';

@Component({
  selector: 'app-todo-list',
  templateUrl: './todo-list.component.html',
  styleUrls: ['./todo-list.component.scss'],
  imports: [NgFor, NgIf, FormsModule, TodoItemComponent],
  standalone: true
})
export class TodoListComponent implements OnInit {
  todos: Todo[] = [];
  filteredTodos: Todo[] = [];
  searchTerm: string = '';

  constructor(
    private todoService: TodoService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadTodos();
  }

  loadTodos(): void {
    this.todoService.getTodos().subscribe({
      next: (todos) => {
        this.todos = todos;
        this.filterTodos();
      },
      error: (error) => {
        console.error('Error loading todos:', error);
        // TODO: Add proper error handling
      }
    });
  }

  filterTodos(): void {
    if (!this.searchTerm) {
      this.filteredTodos = this.todos;
      return;
    }

    const searchTermLower = this.searchTerm.toLowerCase();
    this.filteredTodos = this.todos.filter(todo =>
      todo.title.toLowerCase().includes(searchTermLower) ||
      (todo.description && todo.description.toLowerCase().includes(searchTermLower))
    );
  }

  onStatusChanged(todo: Todo): void {
    this.todoService.toggleTodoStatus(todo.id, !todo.completed).subscribe({
      next: (updatedTodo) => {
        const index = this.todos.findIndex(t => t.id === updatedTodo.id);
        if (index !== -1) {
          this.todos[index] = updatedTodo;
          this.filterTodos();
        }
      },
      error: (error) => {
        console.error('Error updating todo status:', error);
        // TODO: Add proper error handling
      }
    });
  }

  onTodoDeleted(todoId: number): void {
    this.todoService.deleteTodo(todoId).subscribe({
      next: () => {
        this.todos = this.todos.filter(todo => todo.id !== todoId);
        this.filterTodos();
      },
      error: (error) => {
        console.error('Error deleting todo:', error);
        // TODO: Add proper error handling
      }
    });
  }

  navigateToNewTodo(): void {
    this.router.navigate(['/todos/new']);
  }
}
