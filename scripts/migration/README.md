This folder contains an assortment of  files with auxiliary code that was found useful for the process of testing and
verifying the outputs of the migration. It is checked in as reference and for re-use where appropriate, but there are no
automatic mechanisms for running it or tying it with the application code. 

# Analysing migration results

## Copy job results by hand
Currently, the analysis relies on manual copying out of the json presented on the hangfire's jobs pages  out into
individual json files. This obviously doesn't scale super well and although it's been sufficient for needs so far, it's
put a limit on how automatic that process is.
The migrationAnalysisSnippets.js file contains code for merging those json files together and extracting the relevant
information.

## Improved process - automated fetching from the db
Since the hangfire jobs are stored in the db, it's enticing to automate the getting of the results json directly from
there. So far my attempts at this have failed, and it seems like mostly due to the sizes of the result strings being
too much to handle properly by different parts of the process. I hope to come back to this after running a migration
with smaller job sizes (so far we had them at 2-month stretches). See below WIP notes for how to achieve these things.




Migration results are output as part of the hangfire jobs, and can be viewed through the hangfire UI, job by job.
They are json objects, which contain info on each of the processed records and any errors that have prevented a given
record from continuing.

Viewing them job-by-job is not super robust though, so there are a couple of scripts to help A) merge them and 
B) analyse the results.a

## Merging
The job results are saved in hangfire's db, so we can fetch them all at once from there. 

We want to obtain a csv file with these and the most reliable way to achieve that is recorded here. 

1. Export the data to flat file via a quick SSIS package 
- see option 4 in this article: https://www.sqlservercentral.com/articles/8-ways-to-export-sql-results-to-a-text-file
- Select Uniocde on the output options page
- Select tab as the column delimiter. Using comma will interfere with the commas in the data, 
so we'll deal with that conversion next! 
- Use this as the script:
```SQL
SELECT TOP (1000) 
	  [JobId]
      ,[Data]
  FROM [NTBS_Live].[HangFire].[State]
  WHERE [Name] = 'Succeeded'
  AND JobId IN @JobIdsYouAreInterestedId
```
1. Save the results to a csv file.
    1. Copy the raw text of the output file
    1. Paste into excel, press `Ctrl` and select "Import wizard"
    1. Go through the wizard and pay attention to
        - select tab as the source delimiter
        - Make the output columns be "Text" and not "General", o/w excel will wrongly parse the json data in those
        columns
 
 
The consumption of the csv
```js
let firstRow;
let lastRow;
function readAllJsonFromCvs(filePath) {
    const csv = require('csv-parser');
    fs.createReadStream(filePath)
        .pipe(csv())
        .on('data', (row) => {
            lastRow = row;
            if (firstRow == null) firstRow = row;
            
            let data = JSON.parse(row.Data);
            let result = JSON.parse(data.Result);
            console.log(result);
        })
        .on('end', () => {
            console.log('CSV file successfully processed');
        });
}
```