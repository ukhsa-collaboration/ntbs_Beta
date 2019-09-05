# Seeding Data

This folder contains csv data to populate (seed) appropriate tables.

### TB Services 

TB services is the list of TB service codes and names. 
This file contains all entries for TB services and therefore state of the table in database should corresspond to data in this file

### Hospitals

Hospital list contains name of the hospital, together with the id and most importantly corresponding TB service code.
Migration will fail if the TB service with the corresponding code does not exist in database.
Same as with TB Services, data in database should matching this file.
