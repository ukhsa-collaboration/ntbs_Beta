#!/bin/bash

set -e
trap 'read -p "Press enter to continue..."' EXIT

ntbs_dir="ntbs_Beta"

migration_dir="ntbs-data-migration"
migration_sqlproj=".\ntbs-data-migration\ntbs-data-migration.sqlproj"
migration_dacpac=".\ntbs-data-migration\bin\Debug\ntbs-data-migration.dacpac"

reporting_dir="ntbs-reporting"
reporting_sqlproj=".\source\ntbs-reporting.sqlproj"
reporting_dacpac=".\source\bin\Output\ntbs-reporting.dacpac"

specimen_matching_dir="ntbs-specimen-matching"
specimen_matching_sqlproj=".\source\ntbs-specimen-matching.sqlproj"
specimen_matching_dacpac=".\source\bin\Debug\ntbs-specimen-matching.dacpac"

read -p "Which environment would you like to deploy to - live, uat or training (default uat): " DEPLOY_ENVIRONMENT
DEPLOY_ENVIRONMENT=${DEPLOY_ENVIRONMENT:-uat}

if [[ $DEPLOY_ENVIRONMENT = "live" ]]
then
    migration_publish_profile=".\ntbs-data-migration\Publish Profiles\Production\phe-live-migration.publish.xml"
    reporting_publish_profile=".\source\Publish Profiles\Production\phe-live-reporting.publish.xml"
    specimen_matching_publish_profile=".\source\Publish Profiles\Production\phe-live-specimen-matching.publish.xml"
elif [[ $DEPLOY_ENVIRONMENT = "uat" ]]
then
    migration_publish_profile=".\ntbs-data-migration\Publish Profiles\Pre-production\phe-uat-migration.publish.xml"
    reporting_publish_profile=".\source\Publish Profiles\Pre-production\phe-uat-reporting.publish.xml"
    specimen_matching_publish_profile=".\source\Publish Profiles\Pre-production\phe-uat-specimen-matching.publish.xml"
elif [[ $DEPLOY_ENVIRONMENT = "training" ]]
then
    migration_publish_profile=".\ntbs-data-migration\Publish Profiles\Pre-production\azure-training-migration.publish.xml"
    reporting_publish_profile=".\source\Publish Profiles\Pre-production\azure-training-reporting.publish.xml"
    specimen_matching_publish_profile=".\source\Publish Profiles\Pre-production\azure-training-specimen-matching.publish.xml"
elif [[ $DEPLOY_ENVIRONMENT = "int" ]]
then
    migration_publish_profile=".\ntbs-data-migration\Publish Profiles\Pre-production\azure-migration.publish.xml"
    reporting_publish_profile=".\source\Publish Profiles\Pre-production\azure-int-reporting.publish.xml"
    specimen_matching_publish_profile=".\source\Publish Profiles\Pre-production\azure-int-specimen-matching.publish.xml"
elif [[ $DEPLOY_ENVIRONMENT = "dev" ]]
then
    migration_publish_profile=".\ntbs-data-migration\Publish Profiles\Pre-production\DEV-migration.publish.xml"
    reporting_publish_profile=".\source\Publish Profiles\Pre-production\DEV-reporting.publish.xml"
    specimen_matching_publish_profile=".\source\Publish Profiles\Pre-production\DEV-specimen-matching.publish.xml"
elif [[ $DEPLOY_ENVIRONMENT = "test" ]]
then
    migration_publish_profile=".\ntbs-data-migration\Publish Profiles\Pre-production\azure-migration.publish.xml"
    reporting_publish_profile=".\source\Publish Profiles\Pre-production\azure-test-reporting.publish.xml"
    specimen_matching_publish_profile=".\source\Publish Profiles\Pre-production\azure-test-specimen-matching.publish.xml"
elif [[ $DEPLOY_ENVIRONMENT = "azure-uat" ]]
then
    migration_publish_profile=".\ntbs-data-migration\Publish Profiles\Pre-production\azure-uat-migration.publish.xml"
    reporting_publish_profile=".\source\Publish Profiles\Pre-production\azure-uat-reporting.publish.xml"
    specimen_matching_publish_profile=".\source\Publish Profiles\Pre-production\azure-uat-specimen-matching.publish.xml"
else
    echo "Invalid environment entered."
    exit 1
fi

read -p "Are you running this script on the PHE dev box? (default yes): " ON_DEV_BOX
ON_DEV_BOX=${ON_DEV_BOX:-yes}

if [[ $ON_DEV_BOX = "yes" ]]
then
    MSBUILD_PATH="C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe"
    SQLPACKAGE_PATH="C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\IDE\Extensions\Microsoft\SQLDB\DAC\150\sqlpackage.exe"
else
    # This is where these executables are stored on my machine - other users will need to check/update these if running the script locally
    MSBUILD_PATH="C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe"
    SQLPACKAGE_PATH="C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\Common7\IDE\Extensions\Microsoft\SQLDB\DAC\150\sqlpackage.exe"
fi

read -p "Please enter the password for the database connection strings (leave empty if using Windows authentication): " -s PASSWORD

deploy_database() {
    echo -e "\nDeploying $1 to database...\n"
    cd "./$1"
    "$MSBUILD_PATH" "$2"
    if [[ -z "$PASSWORD" ]]
    then
        "$SQLPACKAGE_PATH" -Action:Publish -SourceFile:"$4" -Profile:"$3"
    else
        "$SQLPACKAGE_PATH" -Action:Publish -SourceFile:"$4" -Profile:"$3" -TargetPassword:"$PASSWORD"
    fi
    cd ..
    echo -e "\nSuccessfully deployed $1 to database.\n"
}

deploy_database "$migration_dir" "$migration_sqlproj" "$migration_publish_profile" "$migration_dacpac"
deploy_database "$specimen_matching_dir" "$specimen_matching_sqlproj" "$specimen_matching_publish_profile" "$specimen_matching_dacpac"
deploy_database "$reporting_dir" "$reporting_sqlproj" "$reporting_publish_profile" "$reporting_dacpac"

