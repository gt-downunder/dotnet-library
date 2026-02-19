using System.Security.Claims;

namespace Grondo.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="ClaimsPrincipal"/>.
    /// </summary>
    public static class ClaimsPrincipalEx
    {
        extension(ClaimsPrincipal principal)
        {
            /// <summary>
            /// Gets the value of the first claim with the specified type, or <c>null</c> if no matching claim is found.
            /// </summary>
            /// <param name="claimType">The claim type to search for.</param>
            /// <returns>The claim value, or <c>null</c> if no matching claim exists.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the principal is <c>null</c>.</exception>
            public string? GetClaimValue(string claimType)
            {
                ArgumentNullException.ThrowIfNull(principal);
                ArgumentException.ThrowIfNullOrWhiteSpace(claimType);

                return principal.FindFirst(claimType)?.Value;
            }

            /// <summary>
            /// Determines whether the principal has a claim with the specified type.
            /// </summary>
            /// <param name="claimType">The claim type to check for.</param>
            /// <returns><c>true</c> if the principal has a claim of the specified type; otherwise, <c>false</c>.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the principal is <c>null</c>.</exception>
            public bool HasClaim(string claimType)
            {
                ArgumentNullException.ThrowIfNull(principal);
                ArgumentException.ThrowIfNullOrWhiteSpace(claimType);

                return principal.HasClaim(c => c.Type == claimType);
            }

            /// <summary>
            /// Gets the user ID from the <see cref="ClaimTypes.NameIdentifier"/> claim, or <c>null</c> if not present.
            /// </summary>
            /// <returns>The user ID, or <c>null</c> if not found.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the principal is <c>null</c>.</exception>
            public string? GetUserId() =>
                principal.GetClaimValue(ClaimTypes.NameIdentifier);

            /// <summary>
            /// Gets the email address from the <see cref="ClaimTypes.Email"/> claim, or <c>null</c> if not present.
            /// </summary>
            /// <returns>The email address, or <c>null</c> if not found.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the principal is <c>null</c>.</exception>
            public string? GetEmail() =>
                principal.GetClaimValue(ClaimTypes.Email);

            /// <summary>
            /// Gets the display name from the <see cref="ClaimTypes.Name"/> claim, or <c>null</c> if not present.
            /// </summary>
            /// <returns>The display name, or <c>null</c> if not found.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the principal is <c>null</c>.</exception>
            public string? GetDisplayName() =>
                principal.GetClaimValue(ClaimTypes.Name);
        }
    }
}

