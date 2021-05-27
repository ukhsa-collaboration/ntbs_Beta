import { readFileSync } from 'fs'

const filePath = process.argv[2];

const statsFile = readFileSync(filePath, 'utf8');
const statsJson = statsFile.replace('var stats =', '').split('function')[0].replace(/(\w+):/g, (match, p1) => `"${p1}":`);
const statsObject = JSON.parse(statsJson);

const globalStats = statsObject.stats;
const perRequestStats = Object.values(statsObject.contents);
const dashboardStats = perRequestStats.find(stats => stats.name === 'dashboard').stats;
const idSearchStats = perRequestStats.find(stats => stats.name === 'perform_id_search').stats;
const nameSearchStats = perRequestStats.find(stats => stats.name === 'perform_family_name_search').stats;

const globalErrorRate = globalStats.numberOfRequests.ko / globalStats.numberOfRequests.total;
const globalMean = globalStats.meanResponseTime.total;
const global95thPecentile = globalStats.percentiles3.total;

const dashboardMean = dashboardStats.meanResponseTime.total;
const idSearchMean = idSearchStats.meanResponseTime.total;
const nameSearchMean = nameSearchStats.meanResponseTime.total;

console.log(`Performance metrics:
- Error rate: ${(100 * globalErrorRate).toFixed(2)}%
- Mean response time: ${globalMean}ms
- 95th percentile: ${global95thPecentile}ms
- Mean response time (dashboard): ${dashboardMean}ms
- Mean response time (search by ID): ${idSearchMean}ms
- Mean response time (search by name): ${nameSearchMean}ms
`);

const errors = [];

if (globalErrorRate > 0.001) {
	errors.push(`The error rate ${globalErrorRate} is greater than the threshold 0.001`);
}

if (globalMean > 600) {
	errors.push(`The mean response time ${globalMean}ms is greater than the threshold 600ms`);
}

if (global95thPecentile > 2000) {
	errors.push(`The error rate ${global95thPecentile}ms is greater than the threshold 2000ms`);
}

if (dashboardMean > 1500) {
	errors.push(`The error rate ${dashboardMean}ms is greater than the threshold 1500ms`);
}

if (idSearchMean > 1500) {
	errors.push(`The error rate ${idSearchMean}ms is greater than the threshold 1500ms`);
}

if (nameSearchMean > 1500) {
	errors.push(`The error rate ${nameSearchMean}ms is greater than the threshold 1500ms`);
}

if (errors.length > 0) {
	throw new Error('The load test has failed because one or more thresholds were not met: ' + errors.join(', '));
}
