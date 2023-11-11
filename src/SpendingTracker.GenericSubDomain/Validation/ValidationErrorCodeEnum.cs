namespace SpendingTracker.GenericSubDomain.Validation;

public enum ValidationErrorCodeEnum
{
    Forbidden,
    InternalServerError,
    ObjectWasChanged,
    KeyNotFound,
    RecursivelyAddedCategory,
    CategoriesAlreadyLinked,
    UserAlreadyHasCategoryWithSpecifiedName,
    CategoryDoesNotBelongsToUser,
    CurrentUserHasNoPermissionToDeleteCategory
}