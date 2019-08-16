import 'nhsuk-frontend/packages/nhsuk'
import 'nhsuk-frontend/packages/nhsuk.scss'
import '../css/site.css'
import Vue from 'vue';
import { TestButton } from './Components/TestButton';

// register Vue components
Vue.component('test-button', TestButton);

new Vue({
  el: '#app',
});

// Enables ASP hot relaod
module.hot.accept()