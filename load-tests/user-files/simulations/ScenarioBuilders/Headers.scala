object Headers {
    val get_headers = Map(
        "accept" -> "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9",
        "accept-encoding" -> "gzip, deflate, br",
        "accept-language" -> "en-US,en;q=0.9",
        "sec-ch-ua" -> """ Not A;Brand";v="99", "Chromium";v="90", "Google Chrome";v="90""",
        "sec-ch-ua-mobile" -> "?0",
        "sec-fetch-dest" -> "document",
        "sec-fetch-mode" -> "navigate",
        "sec-fetch-site" -> "same-origin",
        "sec-fetch-user" -> "?1",
        "upgrade-insecure-requests" -> "1",
        "user-agent" -> "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.93 Safari/537.36")

    val validate_headers = Map(
        "accept" -> "application/json, text/plain, */*",
        "accept-encoding" -> "gzip, deflate, br",
        "accept-language" -> "en-US,en;q=0.9",
        "content-type" -> "application/json;charset=UTF-8",
        "origin" -> "https://ntbs-load-test.e32846b1ddf0432eb63f.northeurope.aksapp.io",
        "requestverificationtoken" -> "${requestVerificationToken}",
        "sec-ch-ua" -> """ Not A;Brand";v="99", "Chromium";v="90", "Google Chrome";v="90""",
        "sec-ch-ua-mobile" -> "?0",
        "sec-fetch-dest" -> "empty",
        "sec-fetch-mode" -> "cors",
        "sec-fetch-site" -> "same-origin",
        "user-agent" -> "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.93 Safari/537.36")

    val save_headers = Map(
        "accept" -> "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9",
        "accept-encoding" -> "gzip, deflate, br",
        "accept-language" -> "en-US,en;q=0.9",
        "origin" -> "https://ntbs-load-test.e32846b1ddf0432eb63f.northeurope.aksapp.io",
        "sec-ch-ua" -> """ Not A;Brand";v="99", "Chromium";v="90", "Google Chrome";v="90""",
        "sec-ch-ua-mobile" -> "?0",
        "sec-fetch-dest" -> "document",
        "sec-fetch-mode" -> "navigate",
        "sec-fetch-site" -> "same-origin",
        "sec-fetch-user" -> "?1",
        "upgrade-insecure-requests" -> "1",
        "user-agent" -> "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.93 Safari/537.36")
}
