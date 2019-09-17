import Vue from 'vue';

const BackToTopButton = Vue.extend({
    props: ['model', 'property', 'name'],
    methods: {
        scrollToTop: function() {
            window.scrollTo(0,0);
        }
    }
});

export {
    BackToTopButton
};