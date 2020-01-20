// This file isn't meant to be run directly - instead it's a collection of semi-connected snippets that helped
// with the analysis of the trial migrations that can be run by hand in a node REPL

// Returns concatenated $values from recursively found json files
function readAllJson(dir) {
    const files = fs.readdirSync(dir);
    return files.flatMap(function (file, index) {
        const filePath = `./${path.join(dir, file)}`;
        if (file.indexOf(".json") !== -1) {
            console.log('importing ', filePath);
            return require(filePath).$values
        } else {
            return readAllJson(filePath)
        }
    });
}


let failedGroups = results.filter(v => !v.IsValid);
let allErrors = failedGroups.flatMap(p => p.ValidationErrors).flatMap(ob => Object.values(ob)).flatMap(errors => errors);
// Unique strings, but as some error strings contain ids, this isn't actually the list of encountered error types
let unique = [...new Set(allErrors)];


// knownIssues are strings that uniquely identify types of errors, created manually based of unique
knownIssues.map(issue => {
        let affectedNotifications = failedGroups.flatMap(group => {
            let ids = Object.keys(group.ValidationErrors);
            let affectedIds = ids
                .filter(legacyId => {
                    let notificationErrors = group.ValidationErrors[legacyId] || [];
                    return notificationErrors
                        .some(err => err != null && err.includes(issue))
                });
            if (affectedIds.some(id => id === 'groupFailedToImport')) {
                // Return all, since whole group affected
                return ids.filter(id => id !== 'groupFailedToImport')
            } else return affectedIds
        });
        return ({
            issue,
            numberOfAffactedNotifications: affectedNotifications.length,
            affectedNotifications
        });
    }
)

// Duplicates aren't even validated, so they have slightly different error message shapes:
let duplicateGroups = failedGroups.filter(g => Object.values(g.ValidationErrors)
    .flat().filter(err => err != null).some(error => error.includes('Duplicate records found')))
