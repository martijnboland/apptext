import './styles/test.css';

document.addEventListener('DOMContentLoaded', function () {
  document.getElementById('dynamic').innerHTML = new Date().toLocaleString() + '  ' + window.appText.apiBaseUrl;
});