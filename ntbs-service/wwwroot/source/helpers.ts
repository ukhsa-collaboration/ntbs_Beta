var getValidationBaseBath = function(modelName: string) {
    return `/Validation/Validate${modelName}`;
}

type FormattedDate = { day: any, month: any, year: any };

var convertFormattedDateToDate = function(date: FormattedDate) {
    return new Date(date.year, date.month - 1, date.day)
}

export { 
    getValidationBaseBath, FormattedDate, convertFormattedDateToDate
};