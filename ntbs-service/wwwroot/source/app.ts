// Root styles import - other global styles are imported from this sass file
import "../css/site.scss"
// @ts-ignore
import config from "./config/config-APP_TARGET";
import Vue from "vue";
import { initAll as govUkJsInitAll } from "govuk-frontend";
import '../../node_modules/nhsuk-frontend/packages/components/details/details.polyfill';
import { ValidateInput } from "./Components/ValidateInput";
import { ValidateDate } from "./Components/ValidateDate";
import { DateComparison } from "./Components/DateComparison";
import { YearComparison } from "./Components/YearComparison";
import { ValidateContactTracing } from "./Components/ValidateContactTracing";
import ValidateImmunosuppression from "./Components/ValidateImmunosuppression";
import ValidateTravelOrVisit from "./Components/ValidateTravelOrVisit";
import { ValidateMultiple } from "./Components/ValidateMultiple";
import { ValidateRequiredCheckboxes } from "./Components/ValidateRequiredCheckboxes";
import { ValidatePostcode } from "./Components/ValidatePostcode";
import ConditionalSelectWrapper from "./Components/ConditionalSelectWrapper";
import { AutocompleteSelect } from "./Components/AutocompleteSelect";
import NhsNumberDuplicateWarning from "./Components/NhsNumberDuplicateWarning";
import TbServiceFilteredDropdowns from "./Components/TbServiceFilteredDropdowns";
import TbServiceFilteredAlerts from "./Components/TbServiceFilteredAlerts";

// For compatibility with IE11
require("es6-promise").polyfill();

// Vue needs to be the first thing to load!
// Otherwise, it replaces the templates of its components with fresh content, potentially overwriting changes from other scripts!

// register Vue components
Vue.component("validate-input", ValidateInput);
Vue.component("validate-date", ValidateDate);
Vue.component("date-comparison", DateComparison);
Vue.component("validate-contact-tracing", ValidateContactTracing);
Vue.component("validate-immunosuppression", ValidateImmunosuppression);
Vue.component("validate-travel-or-visit", ValidateTravelOrVisit);
Vue.component("year-comparison", YearComparison);
Vue.component("validate-multiple", ValidateMultiple);
Vue.component("validate-required-checkboxes", ValidateRequiredCheckboxes);
Vue.component("validate-postcode", ValidatePostcode);
Vue.component("conditional-select-wrapper", ConditionalSelectWrapper);
Vue.component("autocomplete-select", AutocompleteSelect);
Vue.component("nhs-number-duplicate-warning", NhsNumberDuplicateWarning);
Vue.component("tb-service-filtered-dropdowns", TbServiceFilteredDropdowns);
Vue.component("tb-service-filtered-alerts", TbServiceFilteredAlerts);

new Vue({
    el: "#app",
});

if (config.env === "development") {
    // Enables ASP hot reload
    console.log("RUNNING IN DEVELOPMENT MODE - Accepting hot reload");
    module.hot.accept();
}

govUkJsInitAll();

