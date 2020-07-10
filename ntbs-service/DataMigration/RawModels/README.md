This folder contains DTO classes with low-level representations of the data coming in from the migration database
These were created to match the state of the migration db's tables and views at the time of writing.

Each class has a comment with the view or table it was based on.

The following script was useful for getting an up-to-date list of the columns:
```SQL
select * from (
  select
    tab.name,
    column_id,
      t.name as data_type,
      col.name as column_name
  from sys.tables as tab
      inner join sys.columns as col
          on tab.object_id = col.object_id
      left join sys.types as t
      on col.user_type_id = t.user_type_id
  union
  
  select object_name(c.object_id) as name,
        column_id,
         type_name(user_type_id) as data_type,
         c.name as column_name
  from sys.columns c
  join sys.views v 
       on v.object_id = c.object_id
  
) tablesAndViews

where name LIKE '' -- Insert name here
     
order by name, column_id
```