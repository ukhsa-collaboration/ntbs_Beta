// Root styles import - other global styles are imported from this sass file
import '../css/site.scss'
// @ts-ignore
import config from './config/config-APP_TARGET';
import Vue from 'vue';
import { initAll as govUkJsInitAll } from 'govuk-frontend';
import { ValidateInput } from './Components/ValidateInput';
import { ValidateDate } from './Components/ValidateDate';
import { DateComparison } from './Components/DateComparison';
import { YearComparison } from './Components/YearComparison';
import { CascadingDropwdowns } from './Components/CascadingDropdowns';
import { ValidateContactTracing } from './Components/ValidateContactTracing';
import { ValidateRiskFactor } from './Components/ValidateRiskFactor';
import { ValidateMultiple } from './Components/ValidateMultiple';
import { ValidateRequiredCheckboxes } from './Components/ValidateRequiredCheckboxes';
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
Vue.component('validate-multiple', ValidateMultiple);
Vue.component('validate-required-checkboxes', ValidateRequiredCheckboxes);

new Vue({
  el: '#app',
});

if (config.env === 'development') {
  // Enables ASP hot relaod
  console.log("RUNNING IN DEVELOPMENT MODE - Accepting hot reload")
  module.hot.accept()
}

govUkJsInitAll();

// For compatibility with IE11
require('es6-promise').polyfill();