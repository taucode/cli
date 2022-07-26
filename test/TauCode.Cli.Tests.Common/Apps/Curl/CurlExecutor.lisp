(executor

    (repeatable
        (key-value
            :keys ("-H")
            :alias "header"
            :token-types ("string")
        )
    )

    (argument
        :alias "url"
        :token-types ("uri"))

    (optional
        (switch
            :keys ("-v" "--verbose")
            :alias "verbose"
        )
    )

    (end)
)
