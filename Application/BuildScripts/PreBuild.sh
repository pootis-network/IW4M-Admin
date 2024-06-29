#!/bin/bash

SolutionDir="$1"
OutputDir="$2"
export PATH="$PATH:~/.dotnet/tools"

if [ ! -d "$SolutionDir/WebfrontCore/wwwroot/lib/font" ]; then
    echo restoring web dependencies
    dotnet tool install Microsoft.Web.LibraryManager.Cli --global
    cd "$SolutionDir/WebfrontCore" || exit
    libman restore
    cd "$SolutionDir" || exit
    cp -r "$SolutionDir/WebfrontCore/wwwroot/lib/open-iconic/font/fonts" "$SolutionDir/WebfrontCore/wwwroot/font"
fi

if [ ! -f "$SolutionDir/WebfrontCore/wwwroot/lib/open-iconic/font/css/open-iconic-bootstrap-override.scss" ]; then
    echo load external resources
    mkdir -p "$SolutionDir/WebfrontCore/wwwroot/lib/open-iconic/font/css"
    curl -o "$SolutionDir/WebfrontCore/wwwroot/lib/open-iconic/font/css/open-iconic-bootstrap-override.scss" https://raw.githubusercontent.com/iconic/open-iconic/master/font/css/open-iconic-bootstrap.scss
    sed -i 's#../fonts/#/font/#g' "$SolutionDir/WebfrontCore/wwwroot/lib/open-iconic/font/css/open-iconic-bootstrap-override.scss"
fi

echo compiling scss files

dotnet tool install Excubo.WebCompiler --global
webcompiler -r "$SolutionDir/WebfrontCore/wwwroot/css/src" -o "$SolutionDir/WebfrontCore/wwwroot/css/" -m disable -z disable
webcompiler "$SolutionDir/WebfrontCore/wwwroot/lib/open-iconic/font/css/open-iconic-bootstrap-override.scss" -o "$SolutionDir/WebfrontCore/wwwroot/css/" -m disable -z disable

if [ ! -f "$SolutionDir/bundle/dotnet-bundle.dll" ]; then
    mkdir -p "$SolutionDir/bundle"
    echo getting dotnet bundle
    curl -o "$SolutionDir/bundle/dotnet-bundle.zip" https://raidmax.org/IW4MAdmin/res/dotnet-bundle.zip
    echo unzipping download
    unzip "$SolutionDir/bundle/dotnet-bundle.zip" -d "$SolutionDir/bundle"
fi
echo executing dotnet-bundle
cd "$SolutionDir/bundle" || exit
dotnet dotnet-bundle.dll clean "$SolutionDir/WebfrontCore/bundleconfig.json"
dotnet dotnet-bundle.dll "$SolutionDir/WebfrontCore/bundleconfig.json"
cd "$SolutionDir" || exit

mkdir -p "$SolutionDir/BUILD/Plugins"
echo building plugins
cd "$SolutionDir/Plugins" || exit 
find . -name "*.csproj" -print0 | xargs -0 -I {} dotnet publish {} -o "$SolutionDir/BUILD/Plugins" --no-restore
cd "$SolutionDir" || exit

if [ ! -d "$OutputDir/Localization" ]; then
    echo downloading translations
    mkdir -p "$OutputDir/Localization"
    localizations=("en-US" "ru-RU" "es-EC" "pt-BR" "de-DE")
    for localization in "${localizations[@]}"
    do
        url="https://master.iw4.zip/localization/$localization"
        filePath="$OutputDir/Localization/IW4MAdmin.$localization.json"
        curl -s "$url" -o "$filePath"
    done
fi

echo copying plugins to buld dir
mkdir -p "$OutputDir/Plugins"
cp -r "$SolutionDir/BUILD/Plugins/*.dll" "$OutputDir/Plugins/"
cp -r "$SolutionDir/Plugins/ScriptPlugins/*.js" "$OutputDir/Plugins/"
