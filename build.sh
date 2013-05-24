#!/bin/bash
set -e

cd $( dirname "$0" )
rm -r */bin */obj || true

if [ ! -d markdownsharp ]
then
	hg clone https://code.google.com/p/markdownsharp/
fi

XBUILDARGS="/p:Configuration=Release" # /p:TargetFrameworkProfile=\"\""

xbuild $XBUILDARGS Scatter.sln

echo  ========================
echo "        ALL DONE"
echo  ========================

