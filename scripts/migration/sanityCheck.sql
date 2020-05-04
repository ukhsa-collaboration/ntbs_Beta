-- report values taken from https://assets.publishing.service.gov.uk/government/uploads/system/uploads/attachment_data/file/821334/Tuberculosis_in_England-annual_report_2019.pdf

SELECT NotificationYear, MigrationCount, 
       -- AnnualReport2019EnglandCount, -- not as useful as its only for England  
       annualDataSet.count asAnnualDataSetCount,
       -- (MigrationCount - AnnualReport2019EnglandCount) as MigrationToAnnualReportDifference,
       (MigrationCount - annualDataSet.count) as MigrationToDatasetDifference
FROM
    (
        SELECT
            DATEPART(YEAR, NotificationDate) AS NotificationYear,
            COUNT(1) AS  [MigrationCount]
        FROM MigrationNotificationsView
        GROUP BY DATEPART(YEAR, NotificationDate)
    ) as MigrationData
        INNER JOIN (VALUES
                    (2018, 4655),
                    (2017, 5070),
                    (2016, 5618),
                    (2015, 5736),
                    (2014, 6473),
                    (2013, 7266),
                    (2012, 8084),
                    (2011, 8280),
                    (2010, 7676),
                    (2009, 8112),
                    (2008, 7809),
                    (2007, 7577),
                    (2006, 7682),
                    (2005, 7658),
                    (2004, 6930),
                    (2003, 6631),
                    (2002, 6675),
                    (2001, 6169),
                    (2000, 6044),
                    (1999, 5939),
                    (1998, 5915),
                    (1997, 5664),
                    (1996, 5493),
                    (1995, 5428)
    ) AS r(y, AnnualReport2019EnglandCount) ON  r.y = NotificationYear
        INNER JOIN (
        SELECT year, count (1) as count FROM [NTBS_Reporting_Staging].[dbo].[ETS_Stata_All]
        WHERE denotified = 0
          AND id2 is not null and id2 != '' -- exclude scotland - for scottish records the id is not duplicated into the id2 field
        group by year
    ) annualDataSet ON NotificationYear = annualDataSet.year
ORDER BY NotificationYear DESC

