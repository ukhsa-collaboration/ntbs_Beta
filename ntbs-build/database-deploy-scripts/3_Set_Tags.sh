#!/bin/bash

set -e
trap 'read -p "Press enter to continue..."' EXIT

ntbs_dir="ntbs_Beta"
migration_dir="ntbs-data-migration"
reporting_dir="ntbs-reporting"
specimen_matching_dir="ntbs-specimen-matching"

read -p "Enter the release number (e.g. 1.2.3): " RELEASE_NUMBER

if [[ -z "$RELEASE_NUMBER" ]]
then
    echo "You must choose a release number."
    exit 1
fi

read -p "Are you running this script on the PHE dev box? (default yes): " ADD_SSH_KEY
ADD_SSH_KEY=${ADD_SSH_KEY:-yes}

set_tag_in_repository() {
    echo -e "\nAdding tag $2 to $1 repository...\n"
    if [[ $ADD_SSH_KEY = "yes" ]]
    then
        git config --add core.sshCommand "ssh -i /E/Scripts/ssh/id_ed25519 -o IdentitiesOnly=yes -o StrictHostKeyChecking=no -p 443"
    fi
    git tag "$2"
    git push origin "$2"
    echo "Successfully added tag $2 to $1 repository."
}

update_repository_tag() {
    echo -e "\nDetermining whether repository $1 requires new tag...\n"
    cd "./$1"
    if git tag --points-at HEAD | grep -q Release
    then
        echo -e "\nRelease tag already exists at HEAD in $1.\n"
    else
        set_tag_in_repository $1 $2
    fi
    cd ..
}

printf -v date_stamp '%(%Y%m%d)T' -1
tag="Release-$RELEASE_NUMBER-$date_stamp"

update_repository_tag $ntbs_dir $tag
update_repository_tag $migration_dir $tag
update_repository_tag $reporting_dir $tag
update_repository_tag $specimen_matching_dir $tag
