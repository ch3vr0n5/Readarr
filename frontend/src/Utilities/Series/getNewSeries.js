
function getNewSeries(series, payload) {
  const {
    rootFolderPath,
    monitor,
    qualityProfileId,
    languageProfileId,
    seriesType,
    seasonFolder,
    tags,
    searchForMissingEpisodes = false
  } = payload;

  const addOptions = {
    monitor,
    searchForMissingEpisodes
  };

  series.addOptions = addOptions;
  series.monitored = true;
  series.qualityProfileId = qualityProfileId;
  series.languageProfileId = languageProfileId;
  series.rootFolderPath = rootFolderPath;
  series.seriesType = seriesType;
  series.seasonFolder = seasonFolder;
  series.tags = tags;

  return series;
}

export default getNewSeries;
