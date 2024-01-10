namespace OpenAiQuickstart.BusinessApi.Queries;

/// <summary>
/// Defines coordinates of a bounding box
/// </summary>
public struct GeoBoundingBox
{
    /// <summary>
    /// Centre Latitude of the bounding circle
    /// </summary>
    public double CentreLatitude { get; set; }
    /// <summary>
    /// Centre Longitude of the bounding circle
    /// </summary>
    public double CentreLongitude { get; set; }
    /// <summary>
    /// Radius in metres to search around
    /// </summary>
    public double BufferInMetres { get; set; }
}