import 'nhsuk-frontend/packages/nhsuk'
import 'nhsuk-frontend/packages/nhsuk.scss'
import '../css/site.css'


const thing = document.getElementById('testButton') as HTMLAnchorElement;
thing.innerText = 'Notify';

// Enables ASP hot relaod
module.hot.accept()