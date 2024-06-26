param (
    [string]$PublishDir = $(throw "-PublishDir is required.")
)

md -Force ("{0}\Localization" -f $PublishDir)

$localizations = @("en-US", "ru-RU", "es-EC", "pt-BR", "de-DE")
foreach($localization in $localizations)
{
    $url = "http://api.raidmax.org:5000/localization/{0}" -f $localization
    $filePath = "{0}\Localization\IW4MAdmin.{1}.json" -f $PublishDir, $localization
    $response = Invoke-WebRequest $url -UseBasicParsing
    Out-File -FilePath $filePath -InputObject $response.Content -Encoding utf8
}
