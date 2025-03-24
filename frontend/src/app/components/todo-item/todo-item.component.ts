import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';
import { NgIf, DatePipe } from '@angular/common';
import { Todo } from '../../models/todo';

@Component({
  selector: 'app-todo-item',
  templateUrl: './todo-item.component.html',
  styleUrls: ['./todo-item.component.scss'],
  imports: [NgIf, DatePipe],
  standalone: true
})
export class TodoItemComponent {
  @Input() todo!: Todo;
  @Output() statusChanged = new EventEmitter<Todo>();
  @Output() deleted = new EventEmitter<number>();

  constructor(private router: Router) {}

  onStatusChange(): void {
    this.statusChanged.emit(this.todo);
  }

  onEdit(): void {
    this.router.navigate(['/todos', this.todo.id, 'edit']);
  }

  onDelete(): void {
    if (confirm('Are you sure you want to delete this todo?')) {
      this.deleted.emit(this.todo.id);
    }
  }
}
