# Database Deploy Scripts 

This directory contains scripts that are used as part of the NTBS release process.

This is a temporary measure to streamline releases in the short term, but it should not be long until these scripts can be replaced by an OpenShift pipeline (and possibly a Github Action too).

## Instructions

In order to run these scripts you should copy them to a root directory which contains checkouts of each of the following repositories:

- ntbs_Beta
- ntbs-data-migration
- ntbs-specimen-matching
- ntbs-reporting

It is important that you have checked out the repositories into folders with the default names. If not, you will need to manually edit the scripts to allow for this.

Then, during the release, simply run the scripts in the correct order, at the appropriate time. They will prompt you for any options that you need to enter.