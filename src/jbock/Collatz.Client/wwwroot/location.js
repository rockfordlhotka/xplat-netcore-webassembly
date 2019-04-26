window.getGeolocation = (instance) => {
    // https://developer.mozilla.org/en-US/docs/Web/API/Geolocation/getCurrentPosition
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition((position) => {
            instance.invokeMethod('Change',
                position.coords.latitude, position.coords.longitude, position.coords.accuracy);
        });
    }
};