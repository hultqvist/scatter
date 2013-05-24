#!/bin/bash
set -e
cd $( dirname "$0" )

if [ ! -d markdownsharp/.hg ]
then
	hg clone https://code.google.com/p/markdownsharp/
fi

cd markdownsharp
hg pull
