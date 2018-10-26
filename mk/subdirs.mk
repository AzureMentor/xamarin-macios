# subdir handling ala automake, but without dealing with auto*

all: all-hook
clean: clean-hook
install: install-hook

all-local clean-local install-local::

all-hook:: all-recurse
clean-hook:: clean-recurse
install-hook:: install-recurse

all-recurse:: all-local
clean-recurse:: clean-local
install-recurse:: install-local

all-recurse clean-recurse install-recurse::
	@for dir in $(SUBDIRS); do \
		echo "Making $(subst -recurse,,$@) in $$dir"; \
		START=$$(perl -MTime::HiRes -e 'printf("%.0f\n",Time::HiRes::time()*1000)'); \
		$(MAKE) -C $$dir $(subst -recurse,,$@); \
		EC=$$?; \
		END=$$(perl -MTime::HiRes -e 'printf("%.0f\n",Time::HiRes::time()*1000)'); \
		printf "Made %s in %s in %i.%.3is\\n" "$(subst -recurse,,$@)" "$$dir" "$$((($$END-$$START)/1000))" "$$((($$END-$$START)%1000))"; \
		if [[ x$$EC != x0 ]]; then exit $$EC; fi \
	done

.PHONY: all-local all-recurse all-hook
.PHONY: clean-local clean-recurse clean-hook
.PHONY: install-local install-recurse install-hook
