import 'nhsuk-frontend/packages/nhsuk'
import 'nhsuk-frontend/packages/nhsuk.scss'
import '../css/site.css'
import Vue from 'vue';
import { TestButton } from './Components/TestButton';
import { AutoSaveInput } from './Components/AutoSaveInput';

// register Vue components
Vue.component('test-button', TestButton);
Vue.component('auto-save-input', AutoSaveInput);

new Vue({
  el: '#app',
});

// Enables ASP hot relaod
module.hot.accept()