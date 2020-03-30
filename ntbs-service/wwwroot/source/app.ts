// The line below ensures that the `module` variable below is resolved to the correct declaration by TS
// - otherwise it clashes with the definition created for Node which doesn't include the `hot` property
///<reference types="webpack-env" />

// Root styles import - other global styles are imported from this sass file
import "../css/site.scss"
// @ts-ignore
import config from "./config/config-APP_TARGET";
import Vue from "vue";
import {initAll as govUkJsInitAll} from "govuk-frontend";
import * as Sentry from '@sentry/browser';
import * as SentryIntegrations from '@sentry/integrations';
// @ts-ignore
import Details from '../../node_modules/nhsuk-frontend/packages/components/details/details';
import VueAccessibleModal from 'vue-accessible-modal'
import cssVars from 'css-vars-ponyfill';
// Components
import ValidateInput from "./Components/ValidateInput";
import ValidateDate from "./Components/ValidateDate";
import DateComparison from "./Components/DateComparison";
import YearComparison from "./Components/YearComparison";
import ValidateContactTracing from "./Components/ValidateContactTracing";
import ValidateImmunosuppression from "./Components/ValidateImmunosuppression";
import ValidateTravelOrVisit from "./Components/ValidateTravelOrVisit";
import ValidateMultiple from "./Components/ValidateMultiple";
import ValidateRequiredCheckboxes from "./Components/ValidateRequiredCheckboxes";
import ValidatePostcode from "./Components/ValidatePostcode";
import ConditionalSelectWrapper from "./Components/ConditionalSelectWrapper";
import AutocompleteSelect from "./Components/AutocompleteSelect";
import NhsNumberDuplicateWarning from "./Components/NhsNumberDuplicateWarning";
import FilteredDropdown from "./Components/FilteredDropdown";
import CascadingDropdown from "./Components/CascadingDropdown";
import TbServiceFilteredAlerts from "./Components/TbServiceFilteredAlerts";
import ValidateRelatedNotification from "./Components/ValidateRelatedNotification";
import NotificationInfo from "./Components/NotificationInfo";
import HideSectionIfFalse from "./Components/HideSectionIfFalse";
import FetchSpecimenPotentialMatch from "./Components/FetchSpecimenPotentialMatch";
import FilteredHomepageKpiDetails from "./Components/FilteredHomepageKpiDetails";
import PrintButton from "./Components/PrintButton";
import FormLeaveChecker from "./Components/FormLeaveChecker";
import ConfirmComponent from "./Components/ConfirmComponent";
import InactivityChecker from "./Components/InactivityChecker";
import InactivityLeaveComponent from "./Components/InactivityLeaveComponent";

// For compatibility with IE11. ArrayFromPolyfill required by vue-accessible-modal.
require("es6-promise").polyfill();
require("./Polyfills/ArrayFromPolyfill");

if (config.env === "production") {
    Sentry.init({
        dsn: 'https://83b245064a684fa7ac86658bf38d2ad3@sentry.io/2862017', // identifies the sentry project
        integrations: [new SentryIntegrations.Vue({Vue, attachProps: true, logErrors: true})],
    });
}

Vue.use(VueAccessibleModal, { transition: "fade" });
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
Vue.component("filtered-dropdown", FilteredDropdown);
Vue.component("cascading-dropdown", CascadingDropdown);
Vue.component("tb-service-filtered-alerts", TbServiceFilteredAlerts);
Vue.component("validate-related-notification", ValidateRelatedNotification);
Vue.component("notification-info", NotificationInfo);
Vue.component("hide-section-if-false", HideSectionIfFalse);
Vue.component("fetch-specimen-potential-match", FetchSpecimenPotentialMatch);
Vue.component("filtered-homepage-kpi", FilteredHomepageKpiDetails);
Vue.component("print-button", PrintButton);
Vue.component("form-leave-checker", FormLeaveChecker);
Vue.component("confirm-component", ConfirmComponent);
Vue.component("inactivity-checker", InactivityChecker);
Vue.component("inactivity-component", InactivityLeaveComponent);

// Vue needs to be the first thing to load!
// Otherwise, it replaces the templates of its components with fresh content, potentially overwriting changes from other scripts!
new Vue({
    el: "#app",
});

if (config.env === "development") {
    // Enables ASP hot reload
    console.log("RUNNING IN DEVELOPMENT MODE - Accepting hot reload");
    module.hot.accept();
}
cssVars();
govUkJsInitAll();

// Initialize NHS components
document.addEventListener('DOMContentLoaded', () => {
    Details();
});
