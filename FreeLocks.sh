#!/bin/bash

git lfs locks | while read -r line
do
    id=$(echo "$line" | awk -F'ID:' '{print $2}' | tr -d ' ')

    if [ -n "$id" ]; then
        echo "Unlocking ID $id"
        git lfs unlock --id "$id"
    fi
done