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

read -p "Which environment would you like to deploy to - live, uat, training, int or dev (default uat): " DEPLOY_ENVIRONMENT
DEPLOY_ENVIRONMENT=${DEPLOY_ENVIRONMENT:-uat}

if [[ $DEPLOY_ENVIRONMENT = "live" ]]
then
    migration_publish_profile=".\ntbs-data-migration\Publish Profiles\Production\PHE-LIVE ntbs-data-migration.publish.xml"
    reporting_publish_profile=".\source\Publish Profiles\DELETE\phe-ntbs-live-reporting-DELETE.publish.xml"
    specimen_matching_publish_profile=".\source\phe-ntbs-live-specimen-matching.publish.xml"
elif [[ $DEPLOY_ENVIRONMENT = "uat" ]]
then
    migration_publish_profile=".\ntbs-data-migration\Publish Profiles\Delete\PHE-UAT ntbs-data-migration.publish.xml"
    reporting_publish_profile=".\source\Publish Profiles\DELETE\phe-ntbs-uat-reporting-DELETE.publish.xml"
    specimen_matching_publish_profile=".\source\phe-ntbs-uat-specimen-matching.publish.xml"
elif [[ $DEPLOY_ENVIRONMENT = "training" ]]
then
    migration_publish_profile=".\ntbs-data-migration\Publish Profiles\Delete\DELETE NTBS-TRAINING ntbs-data-migration.publish.xml"
    reporting_publish_profile=".\source\Publish Profiles\DELETE\azure-ntbs-training-reporting-DELETE.publish.xml"
    specimen_matching_publish_profile=".\source\azure-ntbs-training-specimen-matching.publish.xml"
elif [[ $DEPLOY_ENVIRONMENT = "int" ]]
then
    migration_publish_profile=".\ntbs-data-migration\Publish Profiles\Delete\DELETE NTBS-DEV ntbs-data-migration.publish.xml"
    reporting_publish_profile=".\source\Publish Profiles\DELETE\azure-ntbs-int-reporting-DELETE.publish.xml"
    specimen_matching_publish_profile=".\source\azure-ntbs-int-specimen-matching.publish.xml"
elif [[ $DEPLOY_ENVIRONMENT = "dev" ]]
then
    migration_publish_profile=".\ntbs-data-migration\Publish Profiles\Delete\DELETE DEV ntbs-data-migration.publish.xml"
    reporting_publish_profile=".\source\Publish Profiles\DELETE\DEV-reporting-DELETE.publish.xml"
    specimen_matching_publish_profile=".\source\DEV-specimen-matching.publish.xml"
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

deploy_database() {
    echo -e "\nDeploying $1 to database...\n"
    cd "./$1"
    "$MSBUILD_PATH" "$2"
    "$SQLPACKAGE_PATH" //Action:Publish //Profile:"$3" //SourceFile:"$4"
    cd ..
    echo -e "\nSuccessfully deployed $1 to database.\n"
}

deploy_database "$migration_dir" "$migration_sqlproj" "$migration_publish_profile" "$migration_dacpac"
deploy_database "$reporting_dir" "$reporting_sqlproj" "$reporting_publish_profile" "$reporting_dacpac"
deploy_database "$specimen_matching_dir" "$specimen_matching_sqlproj" "$specimen_matching_publish_profile" "$specimen_matching_dacpac"

