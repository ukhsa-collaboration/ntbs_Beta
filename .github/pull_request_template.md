<!--- Provide a general summary of your changes in the Title above, including the related JIRA ticket -->

## Description
<!--- High level changes overview -->

## Checklist:
- [ ] Automated tests are passing locally.
- [ ] Sanity checked new EF-generated queries for performance.
- [ ] If changing content for notification overview, confirmed renders okay for print in Chrome and IE
### Schema changes
If the NTBS schema changes, that might affect the related components!
- [ ] EF migrations created and committed. Snapshot committed
- [ ] On-demand migration impact considered
- [ ] Reporting database DACPAC updated
- [ ] Specimen matching database DACPAC updated
- [ ] Reporting database ingestion considered (uspGenerateReusableNotification)
### Accessibility testing
Features with UI components should consider items on this list
- [ ] Test functionality without javascript
- [ ] Test in small window (imitating small screen)
- [ ] Check the feature looks and works correctly in IE11
- [ ] Zoom page to 400% - content still visible?
- [ ] Test feature works with keyboard only operation
- [ ] Test with one screen reader (e.g. [NVDA](https://www.nvaccess.org/): see [getting started](https://webaim.org/articles/nvda/)) - logical flow, no unexpected content. 
- [ ] Passes automated checker (e.g. [WAVE](https://chrome.google.com/webstore/detail/wave-evaluation-tool/jbbplnpkjmmeebjpijfedlgcdilocofh))
