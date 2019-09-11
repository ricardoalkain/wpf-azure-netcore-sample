using System.ComponentModel;

namespace TTMS.Data.Common
{
    /// <summary>
    /// Defines how the traveler can interact with timelines
    /// </summary>
    public enum TravelerType
    {
        [Description("")]
        None,
        /// <summary>
        /// Traveler is only allowed to see and document facts.
        /// </summary>
        Observer,
        /// <summary>
        /// Traveler can interact with the timeline only to fix unauthorised changes.
        /// </summary>
        Repairer,
        /// <summary>
        /// Traveler can act in missions to change a specific timeline or define missions
        /// to create ruptures in timelines
        /// </summary>
        Builder,
        /// <summary>
        /// Traveler is trainned and authorised to chase and imprison any person that intentionally changes
        /// a timeline. This level grants permission to move between timelines without previous authorization.
        /// </summary>
        Agent,
        /// <summary>
        /// Defines missions and can fully interact with timelines and time travelers
        /// </summary>
        Director
    }
}
