import {gql} from "@apollo/client";

export const CREATE_CATEGORY = gql`
    mutation NewCategory($categoryName: String!) {
      newCategory(name: $categoryName) {
        categoryId
        name
      }
    }
`;

export const GET_CATEGORIES  = gql`
    query Categories {
      categories {
        categoryId
        name
      }
    }
`;

export const GET_CATEGORY_BY_ID = gql`
    query Category($categoryId: ID!) {
      category(id: $categoryId) {
        categoryId
        name
      }
    }
`;

export const DELETE_CATEGORY_BY_ID = gql`
    mutation DeleteCategory($categoryId: ID!){
      deleteCategory(id: $categoryId) {
        categoryId
        name
      }
    }
`;