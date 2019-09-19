// Root styles import - other global styles are imported from this sass file
import '../css/site.scss'
<<<<<<< HEAD
import { initAll as govUkJsInitAll } from 'govuk-frontend';
import Vue from 'vue';
// import 'nhsuk-frontend/packages/nhsuk.scss'

=======

// @ts-ignore
import config from './config/config-APP_TARGET';
import Vue from 'vue';
import { initAll as govUkJsInitAll } from 'govuk-frontend';
>>>>>>> 741200a5506087c62652b6300015fb27a3432618
import { ValidateInput } from './Components/ValidateInput';
import { ValidateDate } from './Components/ValidateDate';
import { DateComparison } from './Components/DateComparison';
import { YearComparison } from './Components/YearComparison';
import { CascadingDropwdowns } from './Components/CascadingDropdowns';
import { ValidateContactTracing } from './Components/ValidateContactTracing';
import { ValidateRiskFactor } from './Components/ValidateRiskFactor';
// Vue needs to be the first thing to load!
// Otherwise, it replaces the templates of its components with fresh content, potentially overwriting changes from other scripts!

// register Vue components
Vue.component('validate-input', ValidateInput);
Vue.component('validate-date', ValidateDate);
Vue.component('date-comparison', DateComparison);
Vue.component('cascading-dropdowns', CascadingDropwdowns);
Vue.component('validate-contact-tracing', ValidateContactTracing);
Vue.component('validate-riskfactor', ValidateRiskFactor);
Vue.component('year-comparison', YearComparison);

new Vue({
  el: '#app',
});

if (config.env === 'development') {
  // Enables ASP hot relaod
  console.log("RUNNING IN DEVELOPMENT MODE - Accepting hot reload")
  module.hot.accept()
}

govUkJsInitAll()