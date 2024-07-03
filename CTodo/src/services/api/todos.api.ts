import {gql} from "@apollo/client";

export const GET_TODOS = gql`
    query Todos {
      todos {
        todoId
        title
        priority
        isCompleted
        dueDate
        categories {
          categoryId
          name
        }
      }
    }
`;

export const GET_TODO_BY_ID = gql`
    query Todo($todoId: ID!) {
      todo(id: $todoId) {
        todoId
        title
        priority
        isCompleted
        dueDate
        categories {
          categoryId
          name
        }
      }
    }
`;

export const CREATE_TODO = gql`
    mutation NewTodo($title: String!, $priority: String!, $categoryId: ID!, $dueDate: DateTime) {
      newTodo(
        title: $title
        priority: $priority
        category: $categoryId
        dueDate: $dueDate
      ) {
        todoId
        title
        priority
        isCompleted
        dueDate
        categories {
          categoryId
          name
        }
      }
    }
`;

export const TOGGLE_TODO_COMPLETED = gql`
    mutation toggleTodoCompleted($todoId: ID!, $isCompleted: Boolean!) {
      toggleTodoCompleted(
        id: $todoId
        isCompleted: $isCompleted
      ) {
        todoId
        title
        priority
        isCompleted
        dueDate
        categories {
          categoryId
          name
        }
      }
    }
`;

export const DELETE_TODO_BY_ID = gql`
    mutation DeleteTodo($todoId: ID!) {
      deleteTodo(id: $todoId) {
        todoId
        title
        priority
        isCompleted
        dueDate
        categories {
          categoryId
          name
        }
      }
    }
`;