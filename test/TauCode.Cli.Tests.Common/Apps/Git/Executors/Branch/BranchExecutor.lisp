(executor
    (alternatives
        ;======= Deletion =======
        (repeatable
            (key-multi-value
                :keys ("-d" "--delete")
                :alias "branch-to-delete"
                :token-types ("branch-name")
            )
            (key-multi-value
                :keys ("-D")
                :alias "branch-to-hard-delete"
                :token-types ("branch-name")
            )
        )
        ;======= Creation =======
        (sequence
            (argument 
                :is-entrance t
                :alias "branch-to-create-name"
                :token-types ("branch-name")
            )
            (optional
                :is-exit t
                (argument 
                    :alias "branch-to-create-start-ref"
                    :token-types ("ref-name")
                )
            )
        )
        ;======= Enumeration & Options =======
        (repeatable
            (switch
                :keys ("-a" "--all")
                :alias "enumerate-all"
            )
            (switch
                :keys ("-l" "--list")
                :alias "enumerate-list"
            )
            (switch
                :keys ("-q" "--quiet")
                :alias "quiet"
            )
            (switch
                :keys ("-v" "--verbose")
                :alias "verbose"
            )
        )
        ;======= Bad token =======
        (fallback)
    )

    (end)
)
