window.updateChart = (id, data, labels) => {
    var context = document.getElementById(id);
    var chart = new Chart(context, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                label: 'Values',
                data: data,
            }]
        }
    });
};