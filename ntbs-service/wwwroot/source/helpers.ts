var getHeaders = function() {
    let token = (<HTMLInputElement>document.querySelector('[name="__RequestVerificationToken"]'));
    return {
        "RequestVerificationToken": token == null ? token : token.value
    }
}

var getValidationPath = function(modelName: string) {
    return buildPath(`Validate${modelName}`);
}

var buildPath = function(actionPath: string) {
    var currentPath = window.location.pathname;
    var pathWithNoTrailingSlash = currentPath.charAt(currentPath.length-1) === "/" ? currentPath.slice(0, -1) : currentPath;
    return `${pathWithNoTrailingSlash}/${actionPath}`;
}

var buildPathRelativeToOrigin = function(actionPath: string) {
    return `${window.location.origin}/${actionPath}`;
}

type FormattedDate = { day: any, month: any, year: any };

var convertFormattedDateToDate = function(date: FormattedDate) {
    return new Date(date.year, date.month - 1, date.day)
}

export { 
    getHeaders, getValidationPath, FormattedDate, convertFormattedDateToDate, buildPath, buildPathRelativeToOrigin
};