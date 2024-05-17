
function applyFilter(filter, url) {
    let params = "";

    if (filter) {
        params = Object.entries(filter)
            .filter(
            ([key, value]) =>
                value !== null && value !== undefined && value !== ""
            )
            .map(([key, value]) => `${key}=${encodeURIComponent(value)}`)
            .join("&");
        }

    if (params) {
        url += `?${params}`;
      }
      return url;
}

export { applyFilter };