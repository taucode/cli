(executor
    (argument
        :alias "url"
        :token-types ("uri")
    )

    (optional
        (argument
            :alias "path"
            :token-types ("file-path")
        )
    )

    (optional
        (key-value
            :keys ("--branch")
            :alias "branch"
            :token-types ("branch-name")
        )
    )

    (optional
        (switch
            :keys ("--single-branch")
            :alias "single-branch"
        )
    )

    (end)
)
